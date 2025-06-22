FROM node:20 AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

ARG REACT_APP_API_URL

RUN npm run build  

FROM nginx:alpine

WORKDIR  /usr/share/nginx/html

RUN rm -rf ./*

COPY --from=build /app/build .

CMD ["nginx", "-g", "daemon off;"]