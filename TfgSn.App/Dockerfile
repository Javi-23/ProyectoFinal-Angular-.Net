FROM node:16 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

FROM node:16-alpine
WORKDIR /app
COPY --from=build /app/dists/tfg-sn.app /app
COPY server.js /app/server.js
COPY package*.json /app/

RUN npm install --only=production

EXPOSE 80
CMD ["node", "server.js"]