```
docker-compose build --no-cache
docker-compose up 
```
```
docker exec eksperimentalnirad-ab-1 ab -n 1000 -c 10 http://webapp:8080/
This is ApacheBench, Version 2.3 <$Revision: 1903618 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking webapp (be patient)
Completed 100 requests
Completed 200 requests
Completed 300 requests
Completed 400 requests
Completed 500 requests
Completed 600 requests
Completed 700 requests
Completed 800 requests
Completed 900 requests
Completed 1000 requests
Finished 1000 requests


Server Software:        Kestrel
Server Hostname:        webapp
Server Port:            8080

Document Path:          /
Document Length:        3 bytes

Concurrency Level:      10
Time taken for tests:   10.376 seconds
Complete requests:      1000
Failed requests:        0
Total transferred:      136000 bytes
HTML transferred:       3000 bytes
Requests per second:    96.37 [#/sec] (mean)
Time per request:       103.762 [ms] (mean)
Time per request:       10.376 [ms] (mean, across all concurrent requests)
Transfer rate:          12.80 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.3      0       2
Processing:   101  102   0.6    102     105
Waiting:      101  102   0.5    102     104
Total:        101  103   0.6    103     105

Percentage of the requests served within a certain time (ms)
  50%    103
  66%    103
  75%    103
  80%    103
  90%    104
  95%    104
  98%    104
  99%    104
 100%    105 (longest request)
 --------------------------------------------------------------------------------------------------------
docker exec eksperimentalnirad-ab-1 ab -n 1000 -c 10 http://websend:8081/
This is ApacheBench, Version 2.3 <$Revision: 1903618 $>
Copyright 1996 Adam Twiss, Zeus Technology Ltd, http://www.zeustech.net/
Licensed to The Apache Software Foundation, http://www.apache.org/

Benchmarking websend (be patient)
Completed 100 requests
Completed 200 requests
Completed 300 requests
Completed 400 requests
Completed 500 requests
Completed 600 requests
Completed 700 requests
Completed 800 requests
Completed 900 requests
Completed 1000 requests
Finished 1000 requests


Server Software:        Kestrel
Server Hostname:        websend
Server Port:            8081

Document Path:          /
Document Length:        13 bytes

Concurrency Level:      10
Time taken for tests:   0.767 seconds
Complete requests:      1000
Failed requests:        0
Total transferred:      146000 bytes
HTML transferred:       13000 bytes
Requests per second:    1304.36 [#/sec] (mean)
Time per request:       7.667 [ms] (mean)
Time per request:       0.767 [ms] (mean, across all concurrent requests)
Transfer rate:          185.97 [Kbytes/sec] received

Connection Times (ms)
              min  mean[+/-sd] median   max
Connect:        0    0   0.1      0       1
Processing:     3    7   5.8      6      44
Waiting:        3    7   5.8      6      44
Total:          3    8   5.8      6      44

Percentage of the requests served within a certain time (ms)
  50%      6
  66%      7
  75%      7
  80%      8
  90%     10
  95%     17
  98%     35
  99%     36
 100%     44 (longest request)
```
