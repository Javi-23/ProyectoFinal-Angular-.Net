FROM node:16 AS build
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

FROM nginx:alpine
COPY ./nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /app/dists/tfg-sn.app /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
