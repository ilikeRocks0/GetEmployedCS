# Front-end Dockerfile
FROM node:20-alpine AS builder
WORKDIR /app

COPY src/package*.json ./
RUN npm install

COPY src/ ./
RUN npm run build

FROM node:20-alpine AS runner
WORKDIR /app
COPY --from=builder /app/.next ./.next
COPY --from=builder /app/node_modules ./node_modules
COPY --from=builder /app/package.json ./package.json
COPY --from=builder /app/public ./public

EXPOSE 3000
CMD ["npm", "start"]