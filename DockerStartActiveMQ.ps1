docker run --name='activemq' -it --rm `
    -e 'ACTIVEMQ_CONFIG_MINMEMORY=512' `
    -e 'ACTIVEMQ_CONFIG_MAXMEMORY=2048' `
	-e 'ACTIVEMQ_ADMIN_LOGIN=admin' `
	-e 'ACTIVEMQ_ADMIN_PASSWORD=your_password' `
	-p 8161:8161 `
	-p 61616:61616 `
	-p 61613:61613 `
    webcenter/activemq:latest