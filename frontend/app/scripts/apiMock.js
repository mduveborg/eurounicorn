'use strict';

angular.module('frontendAppDev', ['frontendApp', 'ngMockE2E'])
	.run(function ($httpBackend) {

		function regEsc(str) {
			return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\-\\\^\$\|]/g, '\\$&');
		}
		// Pass through request for views
		$httpBackend.whenGET( new RegExp( regEsc( 'views/' ) ) ).passThrough();
		$httpBackend.whenPOST('/api/login').respond({ token: 'dev' });
	});
