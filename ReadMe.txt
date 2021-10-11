Redis setup:
1) run image
	docker run --name my-redis -p 5002:6379 -d redis
2)to interact
	docker exec -it my-redis sh
	redis-cli
	ping
	dbsize

Build and run app container:
1)Create 
	docker build -t heavyeuro/coviddatacollector .
2)Run 
	docker run -p 8080:80 -d heavyeuro/coviddatacollector
3)to interact
	docker ps
	docker stop container id
	docker start container id
	docker push heavyeuro/coviddatacollector
	

