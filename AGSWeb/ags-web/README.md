DEV:
docker run -p 3000:3000 --name ags-web-dev -d ngdh32/ags-web-dev
docker build -t ngdh32/ags-web-dev .

PRODUCTION:
docker build -t ngdh32/ags-web-production -f Dockerfile.production .