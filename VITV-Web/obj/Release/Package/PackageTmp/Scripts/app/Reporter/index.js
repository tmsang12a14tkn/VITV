var indexReporterModule = angular.module("indexReporterModule", []);

indexReporterModule.factory("IndexReporterService", ["$http", function ($http) {
    return {
        GetList: function () {
            return $http.get("/Reporter/GetReporters").then(function (result) {
                return result.data;
            });
        }
    };
}]);

indexReporterModule.filter('offset', function () {
    return function (input, start) {
        if (input) {
            start = +start; //parse to int
            return input.slice(start);
        }
        return [];
    }
});

//indexReporterModule.filter('filterActive', function () {
//    return function (items) {
//        var filtered = [];
//        for (var i = 0; i < items.length; i++) {
//            var item = items[i];
//            if (item.IsShow == false) {
//                filtered.push(item);
//            }
//        }
//        return filtered;
//    };
//});



indexReporterModule.controller("IndexReporterModuleCtrl", ["$scope", "$http", "$timeout", "IndexReporterService", function ($scope, $http, $timeout, IndexReporterService) {
    $scope.itemsPerPage = 12;
    $scope.currentPage = 0;
    $scope.loading = true;
    $scope.orderList = "PositionId";
    function LoadReporters() {
        IndexReporterService.GetList().then(function (data) {
            $scope.list = data.reporters;
            $scope.total = data.total;
        });
        $scope.loading = false;
    }

    $scope.range = function () {
        var rangeSize = 5;
        var ret = [];
        var start, pageCount = $scope.pageCount();
        console.log($scope.Reporters.length);
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
    
    $scope.filter = function () {
        $timeout(function () {
            $scope.total = $scope.Reporters.length;
        }, 10);
    };

    

    //$scope.order = function (orderList) {
    //    $scope.reverse = ($scope.orderList === orderList) ? !$scope.reverse : false;
    //    if ($scope.orderList == 'Active')
    //        $scope.reverse = true;
    //    $scope.orderList = orderList;
    //};

    //$scope.sort_by = function (orderList, event) {
    //    console.log(orderList);
    //    $scope.orderList = orderList;
    //    //if ($scope.orderList == 'Active')
    //    //{
    //    //    $scope.reverse
    //    //    $(event.target).parent().addClass('asc');
    //    //    return 0;
    //    //}
    //    $scope.reverse = !$scope.reverse;

    //    $(event.target).parent().removeClass();
    //    if ($scope.reverse) {
    //        $(event.target).parent().addClass('asc');
    //    } else {
    //        $(event.target).parent().addClass('desc');
    //    }
    //};

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

    LoadReporters();
}]);