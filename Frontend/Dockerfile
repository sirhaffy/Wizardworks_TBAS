FROM node:20-alpine AS build
WORKDIR /app/Frontend

# Copy package.json and yarn.lock
COPY Frontend/package.json Frontend/yarn.lock ./

RUN yarn install --frozen-lockfile

# Copy the rest of the frontend files
COPY Frontend ./

# Build the application
RUN yarn build

# Production image
FROM nginx:alpine
COPY --from=build /app/Frontend/dist /usr/share/nginx/html
COPY Frontend/nginx-default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
