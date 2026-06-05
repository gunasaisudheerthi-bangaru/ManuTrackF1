# Stage 1: Build Angular app
FROM node:22-alpine AS build
WORKDIR /app

# Copy package files and install dependencies (layer cache optimisation)
COPY package*.json ./
RUN npm ci

# Copy source and build for production
COPY . .
RUN npm run build

# Stage 2: Serve with nginx
FROM nginx:alpine AS final
# Copy built Angular output (dist/ManuTrack.UI/browser)
COPY --from=build /app/dist/ManuTrack.UI/browser /usr/share/nginx/html
# Copy custom nginx config (handles SPA routing)
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
