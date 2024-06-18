import request from '@/utils/request'

/**
* 章节管理分页查询
* @param {查询条件} data
*/
export function listChapeter(query) {
  return request({
    url: 'business/Chapeter/list',
    method: 'get',
    params: query,
  })
}
export function selectChapeter() {
  return request({
    url: 'business/Chapeter/select',
    method: 'get',
  })
}
/**
* 新增章节管理
* @param data
*/
export function addChapeter(data) {
  return request({
    url: 'business/Chapeter',
    method: 'post',
    data: data,
  })
}
/**
* 修改章节管理
* @param data
*/
export function updateChapeter(data) {
  return request({
    url: 'business/Chapeter',
    method: 'PUT',
    data: data,
  })
}
/**
* 获取章节管理详情
* @param {Id}
*/
export function getChapeter(id) {
  return request({
    url: 'business/Chapeter/' + id,
    method: 'get'
  })
}

/**
* 删除章节管理
* @param {主键} pid
*/
export function delChapeter(pid) {
  return request({
    url: 'business/Chapeter/delete/' + pid,
    method: 'delete'
  })
}
