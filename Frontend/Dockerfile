FROM node:20-alpine AS build
WORKDIR /app/Frontend

# Build arguments
ARG API_KEY
ARG AZURE_VM_IP

# Set environment variables for build
ENV API_KEY=${API_KEY}
ENV AZURE_VM_IP=${AZURE_VM_IP}

# Copy package.json and yarn.lock
COPY Frontend/package.json Frontend/yarn.lock ./

RUN yarn install --frozen-lockfile

# Copy the rest of the frontend files
COPY Frontend ./

# Build the application
RUN yarn build

# Production image
FROM nginx:alpine

# Copy the built files
COPY --from=build /app/Frontend/dist /usr/share/nginx/html

# Copy and process nginx template
COPY Frontend/nginx.conf.template /etc/nginx/templates/default.conf.template
ENV NGINX_ENVSUBST_TEMPLATE_DIR=/etc/nginx/templates
ENV NGINX_ENVSUBST_OUTPUT_DIR=/etc/nginx/conf.d
ENV AZURE_VM_IP=${AZURE_VM_IP}

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]