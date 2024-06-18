import request from '@/utils/request'

/**
* 课程分页查询
* @param {查询条件} data
*/
export function listCourseService(query) {
  return request({
    url: 'business/CourseService/list',
    method: 'get',
    params: query,
  })
}
export function getselect() {
  return request({
    url: 'business/CourseService/select',
    method: 'get'
  })
}
/**
* 新增课程
* @param data
*/
export function addCourseService(data) {
  return request({
    url: 'business/CourseService',
    method: 'post',
    data: data,
  })
}
/**
* 修改课程
* @param data
*/
export function updateCourseService(data) {
  return request({
    url: 'business/CourseService',
    method: 'PUT',
    data: data,
  })
}
/**
* 获取课程详情
* @param {Id}
*/
export function getCourseService(id) {
  return request({
    url: 'business/CourseService/' + id,
    method: 'get'
  })
}

/**
* 删除课程
* @param {主键} pid
*/
export function delCourseService(pid) {
  return request({
    url: 'business/CourseService/delete/' + pid,
    method: 'delete'
  })
}
