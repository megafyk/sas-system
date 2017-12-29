(function (app) {
    'use strict';
    app.controller('annCtrl', annCtrl);
    annCtrl.$inject = ['$scope', 'apiService']
    function annCtrl($scope, apiService) {
        $scope.pageClass = 'page-announcement';
        $scope.loadingAnnoucements = true;
        $scope.page = 0;
        $scope.pageCount = 0;
        $scope.userClass = "";
        $scope.announcements = [];
        $scope.search = search;
        $scope.options = [
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
        ];
        $scope.ann = {};
        $scope.optionsAdminPop = [
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false },
            { valueID: null, selected: false }
        ];
        $scope.isAllAdPopSelected = false;
        $scope.optionsAdminPopToggledAll = function (isisis) {
            var toggleStatus = !isisis;
            angular.forEach($scope.optionsAdminPop, function (item) {
                item.selected = toggleStatus;
            });
            $scope.isAllAdPopSelected = toggleStatus;
            console.log(toggleStatus);
        }
        $scope.optionAdminPopToggled = function () {
            $scope.isAllAdPopSelected = $scope.optionsAdminPop.every(function (item) {
                return item.selected;
            });
        }
        $scope.toggleAll = function () {
            var toggleStatus = $scope.isAllSelected;
            angular.forEach($scope.options, function (item) {
                item.selected = toggleStatus;
            });
        }

        $scope.optionToggled = function () {
            $scope.isAllSelected = $scope.options.every(function (item) {
                return item.selected;
            });
        }
        $scope.create = create;
        $scope.delete = del;
        $scope.isToAdmin = false;
        $scope.ann = {};
        $scope.clear = clear;
        $scope.close = close;

        $scope.okStaffPop = function () {
            var isConfirm = false;
            isConfirm = confirmAlert("Save changes?");
            if (isConfirm) {
                if ($scope.listRecv[0].Grade == 'All') {
                    $scope.ann.Grade = 'ALL';
                } else {
                    var rcv_group = [];
                    var rcv_class = [];
                    var rcv_user = [];
                    for (var i = 0; i < $scope.listRecv.length; i++) {
                        var str_class = $scope.listRecv[i].Grade + $scope.listRecv[i].Class;
                        if (rcv_group.indexOf($scope.listRecv[i].Grade) == -1) {
                            rcv_group.push($scope.listRecv[i].Grade);
                        }
                        if (rcv_class.indexOf(str_class) == -1) {
                            rcv_class.push(str_class);
                        }
                        if (rcv_user.indexOf($scope.listRecv[i].Name) == -1) {
                            rcv_user.push($scope.listRecv[i].Name);
                        }
                    }
                    rcv_group = rcv_group.sort().join();
                    rcv_class = rcv_class.sort().join();
                    rcv_user = rcv_user.sort().join();
                    $scope.ann.rcv_group = rcv_group;
                    $scope.ann.rcv_class = rcv_class;
                    $scope.ann.rcv_user = rcv_user;
                }
                $scope.showModal = !$scope.showModal;
            }
        }

        $scope.closeStaffPop = function () {
            var isConfirm = false;
            if ($scope.listRecv[0] != null) {
                isConfirm = confirmAlert("Close without saving?");
                if (isConfirm) {
                    $scope.showModal = !$scope.showModal;
                }
            } else {
                $scope.showModal = !$scope.showModal;
            }
        }

        $scope.recipients = recipients;
        $scope.showModal = false;

        $scope.okAdPop = function () {
            var changed = false;
            var str_rcv_copo = '';
            angular.forEach($scope.optionsAdminPop, function (item) {
                changed |= item.selected;
            });
            if (changed) {
                var isConfirm = false;
                isConfirm = confirmAlert("Save changes?");
                if (isConfirm) {
                    if ($scope.ann.isToAdmin) {
                        str_rcv_copo = 'SETA';
                    }
                    else if ($scope.isAllAdPopSelected) {
                        str_rcv_copo = 'ALL';
                    } else {
                        angular.forEach($scope.optionsAdminPop, function (item) {
                            if (item.selected) {
                                str_rcv_copo += item.valueID;
                                str_rcv_copo += ',';
                                item.selected = false;
                            }
                        });
                    }
                    $scope.ann['rcv_copo'] = str_rcv_copo;
                    console.log($scope.ann);
                    $scope.showModal = !$scope.showModal;
                }
            }
        }
        $scope.closeAdPop = function () {
            var changed = false;
            angular.forEach($scope.optionsAdminPop, function (item) {
                changed |= item.selected;
            });
            if (changed) {
                var isConfirm = false;
                isConfirm = confirmAlert("Close without saving?");
                if (isConfirm) {
                    angular.forEach($scope.optionsAdminPop, function (item) {
                        item.selected = false;
                    });

                }
            }
            $scope.showModal = !$scope.showModal;
        }
        function recipients() {
            $scope.showModal = !$scope.showModal;
            if ($scope.userClass == '01' && $scope.showModal) {
                // load data
                loadCopos();
            }
            if ($scope.userClass == '02' && $scope.showModal) {
                // load data
                loadGrades();
                loadClasses();
                $scope.searchStudent();
            }
        }
        $scope.grades = [];
        function loadGrades() {
            var config = {
                params: {}
            };
            apiService.get('api/students/grades/', config, (result) => { $scope.grades = result.data }, () => { console.log('Load grades failed!') })
        }
        $scope.classes = [];
        function loadClasses() {
            var config = {
                params: {}
            };
            apiService.get('api/students/classes', config, (result) => { $scope.classes = result.data }, () => { console.log('Load classes failed!') })
        }
        $scope.clearAllRecvs = function () {
            var isConfirm = false;
            isConfirm = confirmAlert("Clear recipients?");
            if (isConfirm) {
                $scope.listRecv = [];
            }
        }
        $scope.addOptionsStudent = function () {
            for (var i = 0; i < $scope.optionsStudent.length; i++) {
                if ($scope.optionsStudent[i].selected == true) {
                    var str_valueID = $scope.optionsStudent[i].valueID;
                    var isFound = false;
                    for (var j = 0; j < $scope.listRecv.length; j++) {
                        if ($scope.listRecv[j].StudentId == str_valueID) {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound) {
                        for (var j = 0; j < $scope.students.length; j++) {
                            if ($scope.students[j].StudentId == str_valueID) {
                                $scope.listRecv.push($scope.students[j]);
                            }
                        }
                    }

                }
            }
        }
        $scope.removeOptionsStudent = function () {
            if ($scope.listRecv[0].Grade == 'All') {
                $scope.listRecv = [];
            }
            for (var i = 0; i < $scope.optionsStudent.length; i++) {
                if ($scope.optionsStudent[i].selected == true) {
                    var str_valueID = $scope.optionsStudent[i].valueID;
                    $scope.listRecv = $scope.listRecv.filter(function (item) {
                        return item.StudentId !== str_valueID
                    });
                }
            }
        }
        $scope.listRecv = [];
        $scope.selectedGrade = '1';
        $scope.selectedClass = 'A';

        $scope.addAllStudentsOfClass = function () {
            if ($scope.listRecv != null) {
                var config = {
                    params: {
                        page: 0,
                        pageSize: 7,
                        filter: $scope.selectedClass,
                        type: 'class'
                    }
                }
                apiService.get('api/students/', config, loadAllInClassCompleted, () => { console.log('Load students by class failed!') })
                console.log($scope.listRecv)
                for (var i = 0; i < $scope.addInClass.length; i++) {
                    var isFound = false;
                    for (var j = 0; j < $scope.listRecv.length; j++) {
                        if ($scope.listRecv[j].StudentId == $scope.addInClass[i].StudentId) {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound) {
                        $scope.listRecv.push($scope.addInClass[i]);
                    }
                }
            }

        }
        $scope.classChange = function (x) {
            $scope.selectedClass = x;
        }
        $scope.addInClass = [];
        function loadAllInClassCompleted(result) {
            $scope.addInClass = result.data.Items;
        }
        $scope.addAllStudentsOfGrade = function () {
            if ($scope.listRecv != null) {
                var config = {
                    params: {
                        page: 0,
                        pageSize: 7,
                        filter: $scope.selectedGrade,
                        type: 'grade'
                    }
                };
                apiService.get('api/students/', config, loadAllInGradeCompleted, () => { console.log('Load students by grade failed!') })
                for (var i = 0; i < $scope.addInGrade.length; i++) {
                    var isFound = false;
                    for (var j = 0; j < $scope.listRecv.length; j++) {
                        if ($scope.listRecv[j].StudentId == $scope.addInGrade[i].StudentId) {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound) {
                        $scope.listRecv.push($scope.addInGrade[i]);
                    }
                }
            }

        }
        $scope.gradeChange = function (x) {
            $scope.selectedGrade = x;
        }
        $scope.addInGrade = [];
        function loadAllInGradeCompleted(result) {
            $scope.addInGrade = result.data.Items;
        }

        $scope.addAllStudents = function () {
            var isConfirm = false;
            isConfirm = confirmAlert("Add all users to recipients?");
            if (isConfirm) {
                $scope.listRecv = [];
                $scope.listRecv.push({ StudentId: 'All', Grade: 'All', Class: 'All', Name: 'All' });
            }

        }

        $scope.searchStudent = searchStudent;
        $scope.students = [];
        $scope.pageStaffTbl1 = 0;
        $scope.pageCountStaffTbl1 = 0;
        $scope.totalCountStaffTbl1 = 0;
        $scope.loadingStudents = true;
        $scope.optionsStudent = [];
        function searchStudent(page) {
            page = page || 0;
            $scope.loadingStudents = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 7,
                    filter: '',
                    type: ''
                }
            };
            apiService.get('/api/students/', config,
                studentsLoadCompleted,
                () => { console.log('Load students failed!') }
            );
        }
        $scope.toggleAllStudents = function (isisis) {
            var toggleStatus = !isisis;
            angular.forEach($scope.optionsStudent, function (item) {
                item.selected = toggleStatus;
            });
            console.log($scope.optionsStudent);
        }
        $scope.optionStudentToggled = function () {
            $scope.isAllStudentsSelected = $scope.optionsStudent.every(function (item) {
                return item.selected;
            });
        }
        function studentsLoadCompleted(result) {
            $scope.optionsStudent = [];
            $scope.students = result.data.Items;
            for (var i = 0; i < $scope.students.length; i++) {
                $scope.optionsStudent.push({ valueID: $scope.students[i].StudentId, selected: false });
            }
            $scope.pageStaffTbl1 = result.data.Page;
            $scope.pageCountStaffTbl1 = result.data.TotalPages;
            $scope.totalCountStaffTbl1 = result.data.TotalCount;
            $scope.loadingStudents = false;
        }
        $scope.copos = [];
        function loadCopos() {
            var config = {
                params: {
                    value: 1
                }
            };
            apiService.get('/api/coporation/', config, coposLoadCompleted, coposLoadFailed);
        }
        function coposLoadCompleted(result) {
            $scope.copos = result.data;
            for (var i = 0; i < $scope.copos.length; i++) {
                $scope.optionsAdminPop[i].valueID = $scope.copos[i].CoporationCode;
            }
        }
        function coposLoadFailed() {
            console.log('Load copos failed!');
        }

        function clear() {
            var isConfirm = false;
            isConfirm = confirmAlert("Clear all field?");
            if (isConfirm) {
                $scope.ann = {};
            }
        }

        function close() {
            return !confirmAlert("Close without saving?");
        }

        function create() {
            var isConfirm = false;
            isConfirm = confirmAlert("Save this data?");
            if (isConfirm) {
                // create
                if ($scope.ann.isToAdmin) {
                    $scope.ann['rcv_copo'] = "SETA";
                }
                console.log($scope.ann);
                apiService.post('/api/announcement/create',
                    $scope.ann,
                    announcementCreateCompleted,
                    announcementCreateFailed
                );
            }
        }
        function announcementCreateCompleted() {
            $scope.search();
            $scope.ann = {};
            alert('Create new record successfully!');
        }
        function announcementCreateFailed() {
            alert('Failed to create new record!');
        }
        function del() {
            // delete announcement
            var isConfirm = false;
            // show confirmation box
            isConfirm = confirmAlert("Delete this data?");
            if (isConfirm) {
                // delete records
                var config = [];
                for (var i = 0; i < $scope.options.length; i++) {
                    if ($scope.options[i].selected) {
                        config.push($scope.options[i].valueID);
                    }
                }
                if (config.length > 0) {
                    apiService.post('api/announcement/delete',
                        config,
                        announcementsDeleteCompleted,
                        announcementsDeleteFailed
                    );
                } else {
                    announcementsDeleteFailed();
                }
                for (var i = 0; i < $scope.options.length; i++) {
                    $scope.options[i].selected = false;
                }
                $scope.search();
            }
        }

        function announcementsDeleteCompleted() {
            $scope.search();
            alert("Delete successfully");
        }

        function announcementsDeleteFailed() {
            alert("Delete Failed");
        }

        function confirmAlert(str) {
            return confirm(str);
        }

        function search(page) {
            page = page || 0;
            $scope.loadingAnnoucements = true;

            var config = {
                params: {
                    page: page,
                    pageSize: 5,
                    filter: ''
                }
            };
            apiService.get('/api/announcement/', config,
                announcementsLoadCompleted,
                announcementsLoadFailed
            );
        }
        function announcementsLoadCompleted(result) {
            $scope.announcements = result.data.Items;
            $scope.userClass = $scope.announcements[0].UserClass;
            for (var i = 0; i < $scope.announcements.length; i++) {
                $scope.options[i].valueID = $scope.announcements[i].AnnounceID;
            }
            $scope.page = result.data.Page;
            $scope.pagesCount = result.data.TotalPages;
            $scope.totalCount = result.data.TotalCount;
            $scope.loadingAnnoucements = false;
        }
        function announcementsLoadFailed(response) {
            console.log("Load data failed!, Error: " + response.data);
        }
        $scope.search();
    }
})(angular.module('myApp'));