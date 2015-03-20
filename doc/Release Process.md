# Release Process

## Website
- Update copyright year (if needed)
- Use directory "Website/wwwrooot" as "[release]/Website"

- Configure app.ini
	- Set "debugMode = false"
	- Set "testImages = false"

## PanoramaCreator
- Update assembly version
- Update copyright year (if needed)
- Build as release

- Use directory "PanoramaCreator/bin/Release" as "[release]/PanoramaCreator"
- Configure NLog.config   
	- Set "minlevel" of the default logger to "Info"
- Add export of scheduled windows task

## General
- Create "[release]/doc"
	- Add user manual
	- Add link to repository

- Create git tag
- Create release on GitHub
