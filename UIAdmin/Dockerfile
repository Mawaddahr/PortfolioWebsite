# Stage 1: Build the React app
FROM node:24-alpine AS build
# Set the working directory
WORKDIR /UIAdmin
# Copy package.json and package-lock.json to install dependencies
COPY /src /UIAdmin/src
COPY package*.json /UIAdmin
# Install dependencies
RUN npm install
# Copy the rest of the app files
COPY . .

EXPOSE 3000

CMD ["npm", "run", "dev"]
