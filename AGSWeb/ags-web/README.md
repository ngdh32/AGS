DEV:
1. docker build -t ngdh32/ags-web-dev .
2. docker run -p 3000:3000 --name ags-web-dev -d ngdh32/ags-web-dev


PRODUCTION:
docker build -t ngdh32/ags-web-production -f Dockerfile.production .


1. what is agsContext
2. what is InitializePageWithMaster
3. how to construct AgsResponse
4. where to configure the environment variables