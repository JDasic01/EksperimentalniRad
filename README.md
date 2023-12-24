```
docker-compose build --no-cache
docker-compose up 
docker exec eksperimentalnirad-ab-1 ab -n 1000 -c 10 http://webapp:8080/  
docker exec eksperimentalnirad-ab-1 ab -n 1000 -c 10 http://websend:8081/  

docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' webapp
docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' websend
```
