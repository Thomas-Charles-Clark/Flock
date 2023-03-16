window.onmessage = function (e) {
    if (e.data[0] == 'previewURL') {
        PreviewURL.SetText(e.data[1].split('?')[0]);
    }
}

function pad(n, width, z) {
    z = z || '0';
    n = n + '';
    return n.length >= width ? n : new Array(width - n.length + 1).join(z) + n;
}

function SuccessFade(ele) {
    $(ele).animate({ opacity: '0.0' }, 300, 'linear', function () {
        $(this).attr('src', '/images/icons/32/save-success.png').bind('onreadystatechange load', function () {
            $(this).unbind('onreadystatechange load');
            if (this.complete) {
                $(this).animate({ opacity: '1.0' }, 300, 'linear', function () {
                    window.setTimeout(function () {
                        $(ele).animate({ opacity: '0.0' }, 300, 'linear', function () {
                            $(this).attr('src', '/images/icons/32/save.png').bind('onreadystatechange load', function () {
                                $(this).unbind('onreadystatechange load');
                                $(this).animate({ opacity: '1.0' }, 300, 'linear');
                            });
                        });
                    }, 1500);
                });
            }
        });
    });
}

function FailureFade(ele) {
    $(ele).animate({ opacity: '0.0' }, 300, 'linear', function () {
        $(this).attr('src', '/images/icons/32/save-failed.png').bind('onreadystatechange load', function () {
            $(this).unbind('onreadystatechange load');
            if (this.complete) {
                $(this).animate({ opacity: '1.0' }, 300, 'linear', function () {
                    window.setTimeout(function () {
                        $(ele).animate({ opacity: '0.0' }, 300, 'linear', function () {
                            $(this).attr('src', '/images/icons/32/save.png').bind('onreadystatechange load', function () {
                                $(this).unbind('onreadystatechange load');
                                $(this).animate({ opacity: '1.0' }, 300, 'linear');
                            });
                        });
                    }, 2000);
                });
            }
        });
    });
}

function SyncFadeIn(ele) {
    $(ele).animate({ opacity: '0.0' }, 0, 'linear', function () {
        $(this).attr('src', '/images/systemicons/streamline/32/sync-progress.gif').bind('onreadystatechange load', function () {
            $(this).unbind('onreadystatechange load');
            if (this.complete) {
                $(this).animate({ opacity: '1.0' }, 0, 'linear', function () {
                   
                });
            }
        });
    });
}

//function SyncFadeOut(ele) {
//    $(ele).animate({ opacity: '0.0' }, 200, 'linear', function () {
//        $(this).attr('src', '/images/systemicons/streamline/32/sync.png').bind('onreadystatechange load', function () {
//            $(this).unbind('onreadystatechange load');
//            if (this.complete) {
//                $(this).animate({ opacity: '1.0' }, 0, 'linear', function () {

//                });
//            }
//        });
//    });
//}

function SyncFadeSuccess(ele) {
    $(ele).animate({ opacity: '0.0' }, 100, 'linear', function () {
        $(this).attr('src', '/images/systemicons/streamline/32/sync-success.png').bind('onreadystatechange load', function () {
            $(this).unbind('onreadystatechange load');
            if (this.complete) {
                $(this).animate({ opacity: '1.0' }, 100, 'linear', function () {
                    window.setTimeout(function () {
                        $(ele).animate({ opacity: '0.0' }, 300, 'linear', function () {
                            $(this).attr('src', '/images/systemicons/streamline/32/sync.png').bind('onreadystatechange load', function () {
                                $(this).unbind('onreadystatechange load');
                                $(this).animate({ opacity: '1.0' }, 300, 'linear');
                            });
                        });
                    }, 1500);
                });
            }
        });
    });
}

function SyncFadeFailure(ele) {
    $(ele).animate({ opacity: '0.0' }, 100, 'linear', function () {
        $(this).attr('src', '/images/systemicons/streamline/32/sync-failure.gif').bind('onreadystatechange load', function () {
            $(this).unbind('onreadystatechange load');
            if (this.complete) {
                $(this).animate({ opacity: '1.0' }, 100, 'linear', function () {
                    window.setTimeout(function () {
                        $(ele).animate({ opacity: '0.0' }, 300, 'linear', function () {
                            $(this).attr('src', '/images/systemicons/streamline/32/sync.png').bind('onreadystatechange load', function () {
                                $(this).unbind('onreadystatechange load');
                                $(this).animate({ opacity: '1.0' }, 300, 'linear');
                            });
                        });
                    }, 2000);
                });
            }
        });
    });
}

function GreenFade(ele) {
    var col = $(ele).css("background-color");
    $(ele).animate({ 'background-color': '#98f383' }, 100, 'linear', function () {
        window.setTimeout(function () {
            $(ele).animate({ 'background-color': col }, 100, 'linear', function () {

            });
        }, 250)
    });
}

function RedFade(ele) {
    var col = $(ele).css("background-color");
    $(ele).animate({ 'background-color': 'red' }, 200, 'linear', function () {
        $(ele).animate({ 'background-color': col }, 200, 'linear', function () {

        });
    });
}