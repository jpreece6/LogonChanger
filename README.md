# LogonChanger
[![DUB](https://img.shields.io/dub/l/vibe-d.svg?maxAge=2592000?style=flat-square)]()  [![Build Status](https://travis-ci.org/looterwar/LogonChanger.svg?branch=master)](https://travis-ci.org/looterwar/LogonChanger)

Logon changer service for Windows 10. Replaces the logon background with a custom image pulled from disk or bing.

# Important
As of the August 2016 Windows 10 anniversary update the logon changer no longer works due to changes to the way the logon background is managed. The Windows.Ui.Logon.Pri is no longer responsible for managing the background image. 
