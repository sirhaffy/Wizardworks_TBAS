# Uppdaterad Frontend/Dockerfile
FROM node:20-alpine AS build
WORKDIR /app

ARG BUILD_VERSION=local
ENV VITE_BUILD_VERSION=${BUILD_VERSION}

COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

COPY . .
RUN yarn build --mode production

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]