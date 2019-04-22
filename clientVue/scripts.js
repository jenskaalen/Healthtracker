/*jshint esversion: 6 */

Date.prototype.toDateInputValue = (function() {
    var local = new Date(this);
    local.setMinutes(this.getMinutes() - this.getTimezoneOffset());
    return local.toJSON().slice(0, 10);
});

var data = {
    message: 'Hello Vue!',
    inputText: 'write here',
    messages: ['wee', 'woo'],
    chosenFeeling: 5,
    chosenDate: new Date().toDateInputValue(),
    chosenActivity: '',
    chosenComment: '',
    chosenId: 0,
    logEntries: []
};

var app = new Vue({
    el: '#app',
    data: data,
    methods: {
        getEntries: function() {
            const url = 'http://localhost:4003/logs';
            fetch(url)
                .then(response => response.json())
                .then(response => {
                    this.logEntries = response;
                });
        },
        postMessage: function() {
            console.log('ss');
            this.messages.push(this.message);
            this.message = "";
        },
        postLog: function() {
            console.log('balle');
            var method = this.chosenId !== 0 ? 'PUT' : 'POST';

            const url = this.chosenId !== 0 ? 'http://localhost:4003/logs/' + this.chosenId : 'http://localhost:4003/logs';

            let data = {
                    feeling: this.chosenFeeling,
                    date: this.chosenDate,
                    comment: this.chosenComment,
                    activity: this.chosenActivity,
                    //can be removed
                    id: this.chosenId
                }
                // The parameters we are gonna pass to the fetch function
            let fetchData = {
                method: method,
                body: JSON.stringify(data),
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    // "Content-Type": "application/x-www-form-urlencoded",
                },
            }
            fetch(url, fetchData)
                .then(response => response.json())
                .then(response => {
                    console.log('Success:', JSON.stringify(response));
                    this.chosenComment = null;
                    this.chosenActivity = null;
                    this.chosenFeeling = null;
                    this.chosenDate = null;
                    this.chosenId = 0;
                    this.getEntries();
                });
        },
        editLog: function(log) {
            this.chosenComment = log.comment;
            this.chosenActivity = log.activity;
            this.chosenDate = log.date;
            this.chosenId = log.id;
            this.chosenFeeling = log.feeling;
        },
        deleteLog: function(log) {
            var url = 'http://localhost:4003/logs/' + log.id;
            let fetchData = {
                method: 'DELETE',
                body: JSON.stringify(data),
                headers: {
                    "Content-Type": "application/json; charset=utf-8",
                    // "Content-Type": "application/x-www-form-urlencoded",
                }
            }

            //TODO: do something with the result?   
            fetch(url, fetchData)
                .then(response => response.json())
                .then(response => {
                    // console.log(data.logEntries);
                    // data.logEntries = data.logEntries.filter(item => item !== log);
                    // console.log(data.logEntries);
                    console.log('deleted log');
                });

        }
    },
    computed: {
        orderedLogs: function() {
            return _.orderBy(this.logEntries, 'date');
        }
    }
});