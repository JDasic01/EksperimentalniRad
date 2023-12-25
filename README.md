```
docker compose build --no-cache
docker compose up 
```
Starting the benchmark for the app that doesn't use a message broker
```
docker exec ab ab -n 1000 -c 10 http://webapp:8080/
```
Starting the benchmark for the app that uses a message broker
```
docker exec eab ab -n 1000 -c 10 http://websend:8081/
```
