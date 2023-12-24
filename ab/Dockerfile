FROM debian:bullseye-slim

# Install Apache Benchmark and other dependencies
RUN apt-get update \
    && apt-get install -y apache2-utils \
    && rm -rf /var/lib/apt/lists/*

# Set the working directory
WORKDIR /ab

# Copy the contents of the local directory to the working directory
COPY . .

# Set the entry point to the 'ab' command
ENTRYPOINT ["ab"]
