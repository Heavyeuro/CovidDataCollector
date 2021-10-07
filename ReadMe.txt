Redis setup:
1) run image
	docker run --name my-redis -p 5002:6379 -d redis
2)to interact
	docker exec -it my-redis sh
	redis-cli
	ping
	dbsize