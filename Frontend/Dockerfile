FROM node:20-alpine AS build
WORKDIR /app

# Kopiera package.json först för bättre cache
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

# Kopiera resten av filerna
COPY . .

# Bygg applikationen
RUN yarn build

# Produktionsimage
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]