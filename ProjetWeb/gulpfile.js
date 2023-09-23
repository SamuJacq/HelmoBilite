/// <binding BeforeBuild='clean, minimage, minjs, mincss' />
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    uglify = require('gulp-uglify'),
    imagemin = import('gulp-imagemin'),
    del = import('del')
    

var webrootFolder = "wwwroot"
var paths = { webroot: "./" + webrootFolder + "/" }
paths.js = paths.webroot + "js/**/*.js"
paths.css = paths.webroot + "css/**/*.css"
paths.mylib = paths.webroot + "mylib"
paths.image = paths.webroot + "image/**/*.*"

gulp.task("clean", function () { return del.sync(paths.mylib + "/'") })

gulp.task("minjs", function () { return gulp.src(paths.js).pipe(concat("all.min.js")).pipe(uglify()).pipe(gulp.dest(paths.mylib)) })

gulp.task("mincss", function () { return gulp.src(paths.css).pipe(concat("style.css")).pipe(cssmin()).pipe(gulp.dest(paths.mylib)) })

gulp.task("minimage", function () { return gulp.src(paths.image).pipe(imagemin()).pipe(gulp.dest(paths.mylib)) })