
// Thanks to: https://stackoverflow.com/questions/19305821/multiple-modals-overlay
$(document).on('show.bs.modal', '.modal', function () {
    if (typeof Enumerable === 'undefined') {
        var self = $(this);

        var zIndex = 1040 + (10 * $('.modal:visible').not(this).length);

        var adj = 0;

        $('.modal:visible').each(function (i, ele) {
            if (+$(ele).css('z-index') >= zIndex) {
                adj++;
            }
        });

        zIndex += adj * 10;

        console.log(self.attr('id') + ' showing at', zIndex);

        self.css('z-index', zIndex);

        if (self.classList.includes('fade')) {
            window.setTimeout(function () {
                console.log(self.attr('id') + ' backdrop showing at', zIndex - 1);
                $('.modal-backdrop').last().not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        }

    } else {
        var self = $(this);
        var zIndex = 1040;
        if ($('.modal:visible').not(this).length) {
            zIndex = Enumerable.From($('.modal:visible').not(this)).Select(function (x) {
                return +$(x).css('z-index')
            }).Max() + 10;
        }
        console.log(self.attr('id') + ' showing at', zIndex);
        self.css('z-index', zIndex);

        if (self.hasClass('fade')) {
            window.setTimeout(function () {
                console.log(self.attr('id') + ' backdrop showing at', zIndex - 1);
                $('.modal-backdrop').last().not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        }
    }
});

$(document).on('hidden.bs.modal', '.modal', function () {
    $('.modal:visible').length && $(document.body).addClass('modal-open');
});