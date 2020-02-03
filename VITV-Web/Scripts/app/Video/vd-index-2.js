var indexVideoModule = angular.module("indexVideoModule", []);

indexVideoModule.factory("IndexVideoService", ["$http", function ($http) {
    return {
        GetList: function (query, offset, limit, reverse, sortby, rangeType) {
            return $http.get("/Video/GetVideos", {
                params: {
                    search: query,
                    skipRecord: offset,
                    takeRecord: limit,
                    reverse: reverse,
                    sortBy: sortby,
                    rangeType: rangeType
                }
            }).then(function (result) {
                return result.data;
            });
        }
    };
}]);

//indexVideoModule.filter('offset', function () {
//    return function (input, start) {
//        if (input) {
//            start = +start; //parse to int
//            return input.slice(start);
//        }
//        return [];
//    }
//});

indexVideoModule.filter('jsonDate', function ($filter) {
    return function (input, format) {
        return $filter('date')(parseInt(input.substr(6)), format);
    };
});

indexVideoModule.controller("IndexVideoModuleCtrl", ["$scope", "$http", "$timeout", "IndexVideoService", function ($scope, $http, $timeout, IndexVideoService) {
    $scope.itemsPerPage = 10;
    $scope.currentPage = 0;
    $scope.predicate = 'PublishedTime';
    $scope.reverse = true;
    $scope.loading = true;
    $scope.rangeType = 'All';

    function LoadVideos() {
        $scope.$watch("currentPage", function (newValue, oldValue) {
            IndexVideoService.GetList($scope.query, newValue * $scope.itemsPerPage, $scope.itemsPerPage, $scope.reverse, $scope.predicate, $scope.rangeType).then(function (data) {
                $scope.Videos = data.videos;
                $scope.total = data.total;
            });
            $scope.loading = false;
        });
    }

    $scope.editVideo = function (video) {
        window.location.href = "/Video/Edit/" + video.Id;
    };

    $scope.deleteVideo = function (video) {
        window.location.href = "/Video/Delete/" + video.Id;
    };

    $scope.range = function () {
        var rangeSize = 5;
        var ret = [];
        var start, pageCount = $scope.pageCount();

        start = $scope.currentPage;
        if ((start == 0 && pageCount >= rangeSize) || (start < rangeSize && pageCount >= rangeSize)) {
            for (var i = 0; i < rangeSize; i++) {
                ret.push(i);
            }
        } else if (start >= rangeSize && pageCount > rangeSize) {
            start = start + 1;
            for (var i = start - rangeSize; i < start; i++) {
                ret.push(i);
            }
        } else {
            for (var i = 0; i < pageCount; i++) {
                ret.push(i);
            }
        }

        return ret;
    };

    //Client side method
    //$scope.filter = function () {
    //    $timeout(function () {
    //        $scope.total = $scope.Videos.length;
    //    }, 10);
    //};

    //Server side method
    $scope.filter = function () {
        $scope.currentPage = 0;
        $scope.loading = true;
        $scope.loading = 
        LoadVideos();
    };

    $scope.sort_by = function (predicate, event) {
        $scope.predicate = predicate;
        $scope.reverse = !$scope.reverse;

        $(event.target).parent().removeClass();
        if ($scope.reverse) {
            $(event.target).parent().addClass('asc');
        } else {
            $(event.target).parent().addClass('desc');
        }
        LoadVideos();
    };

    $scope.pageCount = function () {
        return Math.ceil($scope.total / $scope.itemsPerPage);
    };

    $scope.prevPage = function () {
        if ($scope.currentPage > 0) {
            $scope.currentPage--;
        }
    };

    $scope.nextPage = function () {
        if ($scope.currentPage < $scope.pageCount() - 1) {
            $scope.currentPage++;
        }
    };

    $scope.setPage = function (n) {
        $scope.currentPage = n;
    };

    LoadVideos();

    document.getElementById("rangeTypeInput").onchange = function () {
        $scope.rangeType = $("#rangeTypeInput").val();
        $scope.diggest();
    };
}]);