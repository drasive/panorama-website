# Release Process

## Website
- Update copyright year (if needed)

- Use directory "Website/wwwrooot" as "[release]/Website"
- Delete temporary files in /images/panoramic
- Configure app.ini
	- Set "debugMode = false"
	- Set "testImages = false"

## PanoramaCreator
- Update assembly version
- Update copyright year (if needed)
- Build as release

- Use directory "PanoramaCreator/bin/Release" as "[release]/PanoramaCreator"
- Delete temporary files in /logs
- Configure NLog.config   
	- Set "minlevel" of the default logger to "Info"

## General
- Create "[release]/doc"
	- Add all applicable files from "/doc"
	- Add link to repository

- Add exports of scheduled windows tasks

- Create git tag
- Create release on GitHub
