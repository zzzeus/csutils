# csutils
A tool to deal with windows information

## APIs

* stop system from sleeping
```

//stop system sleeping
SystemSleepManagement.PreventSleep()

//restore system sleep setting
SystemSleepManagement.ResetSleepTimer()
```

* get system information
```
//get wallpaper file location
Desktop.getWallpaper()

//get the int Pointer to the window which contains the folderview
Desktop.findFolderView()
```