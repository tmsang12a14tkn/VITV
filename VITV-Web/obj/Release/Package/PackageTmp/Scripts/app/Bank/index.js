var indexBankModule = angular.module("indexBankModule", []);

indexBankModule.factory("IndexBankService", ["$http", function ($http) {
    return {
        GetList: function () {
            return $http.get("/Bank/GetBanks").then(function (result) {
                return result.data;
            });
        }
    };
}]);

indexBankModule.filter('offset', function () {
    return function (input, start) {
        if (input) {
            start = +start; //parse to int
            return input.slice(start);
        }
        return [];
    }
});

indexBankModule.controller("IndexBankModuleCtrl", ["$scope", "$http", "$timeout", "IndexBankService", function ($scope, $http, $timeout, IndexBankService) {
    $scope.itemsPerPage = 10;
    $scope.currentPage = 0;
    $scope.loading = true;

    function LoadBanks() {
        IndexBankService.GetList().then(function (data) {
            $scope.list = data.banks;
            $scope.total = data.total;
        });
        $scope.loading = false;
    }

    $scope.editBank = function (Bank) {
        window.location.href = "/Bank/Edit/" + Bank.Id;
    };

    $scope.deleteBank = function (Bank) {
        window.location.href = "/Bank/Delete/" + Bank.Id;
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

    $scope.filter = function () {
        $timeout(function () {
            $scope.total = $scope.Banks.length;
        }, 10);
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

    LoadBanks();
}]);