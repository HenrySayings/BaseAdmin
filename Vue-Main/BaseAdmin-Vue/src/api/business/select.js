import request from '@/utils/request'

/**
* 选择题/多选题分页查询
* @param {查询条件} data
*/
export function listSelect(query) {
  return request({
    url: 'business/Select/list',
    method: 'get',
    params: query,
  })
}

/**
* 新增选择题/多选题
* @param data
*/
export function addSelect(data) {
  return request({
    url: 'business/Select',
    method: 'post',
    data: data,
  })
}
export function ImportSelect(data) {
  return request({
    url: 'business/ImportSelect',
    method: 'post',
    data: data,
  })
}
/**
* 修改选择题/多选题
* @param data
*/
export function updateSelect(data) {
  return request({
    url: 'business/Select',
    method: 'PUT',
    data: data,
  })
}
/**
* 获取选择题/多选题详情
* @param {Id}
*/
export function getSelect(id) {
  return request({
    url: 'business/Select/' + id,
    method: 'get'
  })
}

/**
* 删除选择题/多选题
* @param {主键} pid
*/
export function delSelect(pid) {
  return request({
    url: 'business/Select/delete/' + pid,
    method: 'delete'
  })
}
