# ESTÁGIO 1: Build
# Versão Alpine do Node, que é uma versão Linux leve
FROM node:20-alpine AS build
WORKDIR /usr/src/app

# Copia arquivos de configuração de pacotes primeiro
COPY package*.json ./

# Instala todas as dependências (desenvolvimento, TypeScript)
RUN npm install

# Copia o restante do código fonte
COPY . .

# Roda o compilador do TypeScript para converter o index.ts em index.js
RUN npx tsc

# ESTÁGIO 2: Runtime
FROM node:20-alpine AS final
WORKDIR /usr/src/app

# Copia novamente os arquivos de pacotes
COPY package*.json ./

# Instala as dependências de produção (--omit=dev)
RUN npm install --omit=dev

# Copia o arquivo compilado (.js)
COPY --from=build /usr/src/app/index.js ./

# Sinaliza ao Docker para rodar o arquivo JavaScript nativamente
CMD ["node", "index.js"]