'use strict';

angular.module('frontendAppDev', ['frontendApp', 'ngMockE2E'])
	.run(function ($httpBackend) {

		function regEsc(str) {
			return str.replace(/[\-\[\]\/\{\}\(\)\*\+\?\-\\\^\$\|]/g, '\\$&');
		}
		// Pass through request for views and images
		$httpBackend.whenGET( new RegExp( regEsc( 'views/' ) ) ).passThrough();
		$httpBackend.whenGET( new RegExp( regEsc( 'images/' ) ) ).passThrough();

		//override this route with a response
		$httpBackend.whenPOST('/api/login').respond({ token: 'dev' });

		$httpBackend.whenGET('/api/submissions').passThrough();
	});
