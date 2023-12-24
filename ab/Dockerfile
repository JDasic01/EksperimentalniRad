# Use the httpd image as the base image
FROM httpd:latest

# Install Apache Benchmark (ab)
RUN apt-get update && \
    apt-get install -y apache2-utils && \
    rm -rf /var/lib/apt/lists/*

# Expose the default HTTP port
EXPOSE 80

# Set the default command to start Apache
CMD ["httpd-foreground"]
