FROM node:16.14-alpine AS build
WORKDIR /dist/src/app
RUN npm cache clean --force

COPY . .

RUN npm install
RUN npm run build --prod

FROM nginx:alpine
COPY --from=build /app/dists/tfg-sn.app /usr/share/nginx/html
COPY /nginx.conf  /etc/nginx/conf.d/default.conf
EXPOSE 80