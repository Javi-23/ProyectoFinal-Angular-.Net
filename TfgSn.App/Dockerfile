
FROM node:16-alpine AS build
WORKDIR /dist/src/app
RUN npm cache clean --force

COPY . .

RUN npm install
RUN npm run build --prod

FROM nginx:latest AS ngi
COPY --from=build /dist/src/app/dists/my-docker-angular-app /usr/share/nginx/html
COPY /nginx.conf  /etc/nginx/conf.d/default.conf
EXPOSE 80