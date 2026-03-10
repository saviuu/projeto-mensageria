import pg from 'pg';
import * as amqp from 'amqplib';

const RABBITMQ_URL = process.env.RABBITMQ_URL || 'amqp://admin:adminpassword@localhost:5672';
const PG_URL = process.env.PG_URL || 'postgresql://admin:adminpassword@localhost:5432/relatorios_db';
const QUEUE_NAME = process.env.QUEUE_NAME || 'relatorios_queue';

async function processarRelatorio(dbClient: pg.Client, relatorioId: string): Promise<void> {
    // Atualiza o status para "Processando"
    await dbClient.query(
        `UPDATE "Relatorios" SET "Status" = $1 WHERE "Id" = $2`,
        ['Processando', relatorioId]
    );

    console.log(`Processando relatório... (simulando operação pesada de 5 segundos)`);
    await new Promise(resolve => setTimeout(resolve, 5000));

    // Atualiza o status para "Concluido" e preenche a DataConclusao
    await dbClient.query(
        `UPDATE "Relatorios" SET "Status" = $1, "DataConclusao" = $2 WHERE "Id" = $3`,
        ['Concluido', new Date(), relatorioId]
    );
}

async function startWorker() {
    // Conexão com o PostgreSQL
    const dbClient = new pg.Client({ connectionString: PG_URL });
    await dbClient.connect();
    console.log('Conectado ao PostgreSQL com sucesso.');

    // Conexão com o RabbitMQ
    const connection = await amqp.connect(RABBITMQ_URL);
    const channel = await connection.createChannel();
    
    await channel.assertQueue(QUEUE_NAME, { durable: true });
    console.log(`Aguardando mensagens na fila: ${QUEUE_NAME}...`);

    channel.consume(QUEUE_NAME, async (msg: amqp.ConsumeMessage | null) => {
        if (!msg) return;

        try {
            const content = JSON.parse(msg.content.toString());
            const relatorioId = content.RelatorioId;

            console.log(`\n[x] Nova solicitação recebida. Relatório ID: ${relatorioId}`);
            await processarRelatorio(dbClient, relatorioId);

            console.log(`[v] Relatório ${relatorioId} processado com sucesso.`);
            channel.ack(msg);

        } catch (error) {
            console.error('Erro ao processar mensagem:', error);
            channel.nack(msg, false, false); 
        }
    });
}

// Inicia o Worker
startWorker().catch(console.error);