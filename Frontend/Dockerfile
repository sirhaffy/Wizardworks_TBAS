FROM node:20-alpine AS build

WORKDIR /app

# Installera dependencies
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

# Kopiera källkod och bygg
COPY . .
RUN yarn build

# Produktions image
FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
