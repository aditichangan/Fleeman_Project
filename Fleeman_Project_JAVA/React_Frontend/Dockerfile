# Build stage
FROM node:20-alpine AS build
WORKDIR /app

# Set build-time environment variable for Vite
ARG VITE_API_URL=http://localhost:9001
ENV VITE_API_URL=$VITE_API_URL

COPY package*.json ./
RUN npm install
COPY . .
RUN npm run build

# Production stage
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
# Copy a default nginx config if needed, but for now, we'll use default
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
