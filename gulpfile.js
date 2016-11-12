var webpack = require('webpack-stream');
var gulp = require('gulp');
var webpackConfig = require('./webpack.config.js');
var plumber = require('gulp-plumber');

gulp.task('default', function(){
    return gulp.src('.')
  .pipe(webpack(webpackConfig))
  .pipe(gulp.dest('.'));
});