# Stage 1: Build the React app
FROM node:24-alpine AS build
# Set the working directory
WORKDIR /UI
# Copy package.json and package-lock.json to install dependencies
COPY package*.json /UI
# Install dependencies
RUN npm install
# Copy the rest of the app files
COPY . .

EXPOSE 5000

CMD ["npm", "run", "dev"]
