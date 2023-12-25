```
git clone git@github.com:JDasic01/EksperimentalniRad.git
cd EksperimentalniRad
docker compose build --no-cache
docker compose up 
```
After docker compose up command, wait for this output 
```
eksperimentalnirad-webreceive-1  |  [*] Waiting for messages.
eksperimentalnirad-webreceive-1  | info: Microsoft.Hosting.Lifetime[0]
eksperimentalnirad-webreceive-1  |       Application started. Press Ctrl+C to shut down.
eksperimentalnirad-webreceive-1  | info: Microsoft.Hosting.Lifetime[0]
eksperimentalnirad-webreceive-1  |       Hosting environment: Production
eksperimentalnirad-webreceive-1  | info: Microsoft.Hosting.Lifetime[0]
eksperimentalnirad-webreceive-1  |       Content root path: /WebAppReceive
```

In a new terminal start the benchmark for the app that doesn't use a message broker
```
docker exec ab ab -n 1000 -c 10 http://webapp:8080/
```
Starting the benchmark for the app that uses a message broker
```
docker exec ab ab -n 1000 -c 10 http://websend:8081/
```
