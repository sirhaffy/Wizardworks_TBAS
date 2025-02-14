FROM node:20-alpine AS build
WORKDIR /app

# Kopiera package.json först
COPY package.json yarn.lock ./
RUN yarn install --frozen-lockfile

# Kopiera alla filer inklusive .env
COPY . .

# Sätt miljövariabler explicit för byggprocessen
ENV VITE_API_BASE_URL=http://localhost/api
ENV VITE_API_KEY=FmNtbofyWyd4O22y5rNPxHplkY5YCtUI9IcL3ADqhyE

# Bygg applikationen
RUN yarn build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]