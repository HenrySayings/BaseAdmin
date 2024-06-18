import request from '@/utils/request'

/**
* 成绩表分页查询
* @param {查询条件} data
*/
export function listUserScore(query) {
  return request({
    url: 'business/UserScore/list',
    method: 'get',
    params: query,
  })
}

/**
* 新增成绩表
* @param data
*/
export function addUserScore(data) {
  return request({
    url: 'business/UserScore',
    method: 'post',
    data: data,
  })
}
/**
* 修改成绩表
* @param data
*/
export function updateUserScore(data) {
  return request({
    url: 'business/UserScore',
    method: 'PUT',
    data: data,
  })
}
/**
* 获取成绩表详情
* @param {Id}
*/
export function getUserScore(id) {
  return request({
    url: 'business/UserScore/' + id,
    method: 'get'
  })
}

/**
* 删除成绩表
* @param {主键} pid
*/
export function delUserScore(pid) {
  return request({
    url: 'business/UserScore/delete/' + pid,
    method: 'delete'
  })
}
