import request from '@/utils/request'

/**
* 试卷表分页查询
* @param {查询条件} data
*/
export function listExam(query) {
  return request({
    url: 'business/Exam/list',
    method: 'get',
    params: query,
  })
}

/**
* 新增试卷表
* @param data
*/
export function addExam(data) {
  return request({
    url: 'business/Exam',
    method: 'post',
    data: data,
  })
}
/**
* 修改试卷表
* @param data
*/
export function updateExam(data) {
  return request({
    url: 'business/Exam',
    method: 'PUT',
    data: data,
  })
}
/**
* 获取试卷表详情
* @param {Id}
*/
export function getExam(id) {
  return request({
    url: 'business/Exam/' + id,
    method: 'get'
  })
}

/**
* 删除试卷表
* @param {主键} pid
*/
export function delExam(pid) {
  return request({
    url: 'business/Exam/delete/' + pid,
    method: 'delete'
  })
}
