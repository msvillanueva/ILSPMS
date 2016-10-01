(function() {
    'use strict';
	
    var cApp = angular.module('cambria', []);
    cApp.factory('cambria', cambria);

    cambria.$inject = ['$compile', '$rootScope', '$window'];

    function cambria($compile, $rootScope, $window) {
        var fnCallback;

        function _show(title, msg, type, callback) {
            fnCallback = callback;
            _hide();

            var _body = angular.element(document.querySelector('html'))[0];
            var _docBody = angular.element(document.querySelector('body'));
            var _height = _docBody.offsetHeight > _body.offsetHeight ? _docBody.offsetHeight : _body.offsetHeight;
            var _width = _docBody.clientWidth > _body.clientWidth ? _docBody.clientWidth : _body.clientWidth;
            _docBody.append('<div id="bg_box_generated"></div>');

            _docBody.append(
                '<div id="msg_box_container_generated" class="panel panel-default">' +
                    '<div id="msg_box_title_generated" class="panel-heading"></div>' +
                    '<div id="msg_box_content_generated" class="panel-body">' +
                        '<br/>' +
                        '<div class="row">' +
                            '<div id="msg_panel_generated" class="col-lg-12"></div>' +
                        '</div>' +
                        '<br/>' +
                        '<br/>' +
                        '<div class="row">' +
                            '<div id="button_container_generated" class="text-center col-lg-12"></div>' +
                        '</div>' +
                    '</div>' +
                    
                '</div>');

            var scope = $rootScope.$new();
            scope.hideBox = hideBox;

            var btnGen;
            switch (type) {
                case 'alert':
                    var btnGen = '<input type="button" class="btn btn-primary" id="btnOk_generated" value="OK" ng-click="hideBox(true)" />';
                    break;
                case 'confirm':
                    var btnGen = '<input type="button" class="btn btn-primary" id="btnYes_generated" value="YES" ng-click="hideBox(true)"/>' +
                        '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' +
                        '<input type="button" class="btn btn-default" id="btnNo_generated" value="NO" ng-click="hideBox(false)"/>';
                    break;
            }

            var temp = $compile(btnGen)(scope);
            angular.element(document.querySelector('#button_container_generated')).append(temp);
            angular.element(document.querySelector('#msg_box_title_generated')).append('<h3 class="panel-title">' + title + '</h3>');
            angular.element(document.querySelector('#msg_panel_generated')).append(msg);

            var box = angular.element(document.querySelector('#msg_box_container_generated'))[0];
            var _top = (_body.offsetHeight / 2) - (box.offsetHeight / 2);
            _top = _top > 0 ? _top : 100;
            var _left = (_body.clientWidth / 2) - (box.clientWidth / 2);

            document.getElementById('msg_box_container_generated').style.top = _top + 'px';
            document.getElementById('msg_box_container_generated').style.left = _left + 'px';

            box.top = _top;
            box.left = _left;
        }

        function _hide() {
            angular.element(document.querySelector('#bg_box_generated')).remove();
            angular.element(document.querySelector('#msg_box_container_generated')).remove();
        }

        function cAlert(message, title, callback) {
            if (title == null) title = 'Alert';
            if (($.trim(message)).length == 0) message = "&nbsp;";
            _show(title, message, 'alert', function (result) {
                if (callback) callback(result);
            });
        }

        function cConfirm(message, title, callback) {
            if (title == null) title = 'Confirm';
            if (($.trim(message)).length == 0) message = "&nbsp;";
            _show(title, message, 'confirm', function (result) {
                if (callback) callback(result);
            });
        }

        function hideBox(ok) {
            if (ok)
                fnCallback(true);
            _hide();
        }

        return {
            cAlert: cAlert,
            cConfirm: cConfirm
        }

    };
	
})();