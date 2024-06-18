<!--
 * @Descripttion: (章节管理/Chapeter)
 * @Author: (admin)
 * @Date: (2024-05-01)
-->
<template>
  <div>
    <!-- 工具区域 -->
    <el-row :gutter="15" class="mb10">
      <el-col :span="1.5">
        <el-button type="primary" v-hasPermi="['chapeter:add']" plain icon="plus" @click="handleAdd">
          {{ $t('btn.add') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="success" :disabled="single" v-hasPermi="['chapeter:edit']" plain icon="edit" @click="handleUpdate">
          {{ $t('btn.edit') }}
        </el-button>
      </el-col>
      <el-col :span="1.5">
        <el-button type="danger" :disabled="multiple" v-hasPermi="['chapeter:delete']" plain icon="delete" @click="handleDelete">
          {{ $t('btn.delete') }}
        </el-button>
      </el-col>
      <right-toolbar v-model:showSearch="showSearch" @queryTable="getList" :columns="columns"></right-toolbar>
    </el-row>

    <el-table
      :data="dataList"
      v-loading="loading"
      ref="table"
      border
      header-cell-class-name="el-table-header-cell"
      highlight-current-row
      @sort-change="sortChange"
      @selection-change="handleSelectionChange">
      <el-table-column type="selection" width="50" align="center" />
      <el-table-column prop="chapterName" label="章节名称" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('chapterName')" />
      <el-table-column
        prop="chapterItemName"
        label="节点名称"
        align="center"
        :show-overflow-tooltip="true"
        v-if="columns.showColumn('chapterItemName')" />
      <el-table-column prop="course" label="课程名" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('course')" />
      <el-table-column prop="chapterUrl" label="视频路径" align="center" v-if="columns.showColumn('chapterUrl')"> </el-table-column>
      <el-table-column prop="desc" label="描述" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('desc')" />
      <el-table-column prop="createUser" label="创建人" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('createUser')" />
      <el-table-column prop="createTime" label="创建时间" :show-overflow-tooltip="true" v-if="columns.showColumn('createTime')" />
      <el-table-column prop="updateUser" label="修改人" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('updateUser')" />
      <el-table-column prop="updateTime" label="修改时间" :show-overflow-tooltip="true" v-if="columns.showColumn('updateTime')" />
      <el-table-column label="操作" width="160">
        <template #default="scope">
          <el-button type="primary" size="small" icon="view" title="详情" @click="handlePreview(scope.row)"></el-button>
          <el-button
            type="success"
            size="small"
            icon="edit"
            title="编辑"
            v-hasPermi="['chapeter:edit']"
            @click="handleUpdate(scope.row)"></el-button>
          <el-button
            type="danger"
            size="small"
            icon="delete"
            title="删除"
            v-hasPermi="['chapeter:delete']"
            @click="handleDelete(scope.row)"></el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize" @pagination="getList" />

    <!-- 添加或修改章节管理对话框 -->
    <el-dialog :title="title" :lock-scroll="false" v-model="open">
      <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-row :gutter="20">
          <el-col :lg="13">
            <el-form-item label="章节名称" prop="chapterName">
              <el-input v-model="form.chapterName" placeholder="请输入章节名称" />
            </el-form-item>
          </el-col>
          <el-col :lg="13">
            <el-form-item label="子节点名称" prop="chapterItemName">
              <el-input v-model="form.chapterItemName" placeholder="请输入章节名称" />
            </el-form-item>
          </el-col>
          <el-col :lg="13">
            <el-form-item label="课程名称" prop="course">
              <el-select v-model="form.courseId" placeholder="请选择课程">
                <el-option v-for="item in optionsBy" :key="item.courseId" :label="item.courseName" :value="item.courseId" />
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :lg="24">
            <el-form-item label="上传文件" prop="chapterUrl">
              <UploadFile v-model="form.chapterUrl" :data="{ uploadType: 1 }" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="描述" prop="desc">
              <el-input v-model="form.desc" placeholder="请输入" :autosize="{ minRows: 2, maxRows: 4 }" type="textarea" />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
      <template #footer v-if="opertype != 3">
        <el-button text @click="cancel">{{ $t('btn.cancel') }}</el-button>
        <el-button type="primary" @click="submitForm">{{ $t('btn.submit') }}</el-button>
      </template>
    </el-dialog>
  </div>
</template>

<script setup name="chapeter">
import { reactive, onMounted } from 'vue'
import { listChapeter, addChapeter, delChapeter, updateChapeter, getChapeter } from '@/api/business/chapeter.js'
import { getselect } from '@/api/business/courseservice.js'
const { proxy } = getCurrentInstance()
const ids = ref([])
const loading = ref(false)
const showSearch = ref(true)
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  sort: 'ChapterName',
  sortType: 'asc',
  chapterName: undefined
})
const columns = ref([
  { visible: true, prop: 'chapeterId', label: '章节编号' },
  { visible: true, prop: 'chapterName', label: '章节名称' },
  { visible: true, prop: 'chapterItemName', label: '节点名称' },
  { visible: true, prop: 'course', label: '课程名' },
  { visible: true, prop: 'chapterUrl', label: '文件路径' },
  { visible: true, prop: 'desc', label: 'Desc' },
  { visible: true, prop: 'createUser', label: '创建人' },
  { visible: false, prop: 'createTime', label: '创建时间' },
  { visible: false, prop: 'updateUser', label: '修改人' },
  { visible: false, prop: 'updateTime', label: '修改时间' }
])
const total = ref(0)
const dataList = ref([])
const queryRef = ref()
const defaultTime = ref([new Date(2000, 1, 1, 0, 0, 0), new Date(2000, 2, 1, 23, 59, 59)])

var dictParams = []

function getList() {
  loading.value = true
  listChapeter(queryParams).then((res) => {
    const { code, data } = res
    if (code == 200) {
      dataList.value = data.result
      total.value = data.totalNum
      loading.value = false
    }
  })
}

// 查询
function handleQuery() {
  queryParams.pageNum = 1
  getList()
}
const optionsBy = ref([])
function OptionsByCourse() {
  loading.value = true
  getselect()
    .then((res) => {
      const { code, data } = res
      if (code === 200) {
        optionsBy.value = data
      }
      loading.value = false
    })
    .catch((error) => {
      console.error('Error fetching options:', error)
      loading.value = false
    })
}
// 重置查询操作
function resetQuery() {
  proxy.resetForm('queryRef')
  handleQuery()
}
// 多选框选中数据
function handleSelectionChange(selection) {
  ids.value = selection.map((item) => item.id)
  single.value = selection.length != 1
  multiple.value = !selection.length
}
// 自定义排序
function sortChange(column) {
  var sort = undefined
  var sortType = undefined

  if (column.prop != null && column.order != null) {
    sort = column.prop
    sortType = column.order
  }
  queryParams.sort = sort
  queryParams.sortType = sortType
  handleQuery()
}

/*************** form操作 ***************/
const formRef = ref()
const title = ref('')
// 操作类型 1、add 2、edit 3、view
const opertype = ref(0)
const open = ref(false)
const state = reactive({
  single: true,
  multiple: true,
  form: {},
  rules: {},
  options: {
    // IsDeleted 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
    isDeletedOptions: []
  }
})

const { form, rules, options, single, multiple } = toRefs(state)

// 关闭dialog
function cancel() {
  open.value = false
  reset()
}

// 重置表单
function reset() {
  form.value = {
    chapeterId: null,
    chapterName: null,
    chapterItemName: null,
    courseId: null,
    course: null,
    chapterUrl: null,
    desc: null,
    createUser: null,
    createTime: null,
    updateUser: null,
    updateTime: null,
    isDeleted: null
  }
  proxy.resetForm('formRef')
}

/**
 * 查看
 * @param {*} row
 */
function handlePreview(row) {
  reset()
  const id = row.chapeterId
  getChapeter(id).then((res) => {
    const { code, data } = res
    if (code == 200) {
      open.value = true
      title.value = '查看'
      opertype.value = 3
      form.value = {
        ...data
      }
    }
  })
}

// 添加按钮操作
function handleAdd() {
  reset()
  open.value = true
  title.value = '添加章节'
  opertype.value = 1
}
// 修改按钮操作
function handleUpdate(row) {
  reset()
  const id = row.chapeterId || ids.value
  getChapeter(id).then((res) => {
    const { code, data } = res
    if (code == 200) {
      open.value = true
      title.value = '修改章节'
      opertype.value = 2

      form.value = {
        ...data
      }
    }
  })
}

// 添加&修改 表单提交
function submitForm() {
  proxy.$refs['formRef'].validate((valid) => {
    if (valid) {
      if (form.value.chapeterId != undefined && opertype.value === 2) {
        updateChapeter(form.value).then((res) => {
          proxy.$modal.msgSuccess('修改成功')
          open.value = false
          getList()
        })
      } else {
        addChapeter(form.value).then((res) => {
          proxy.$modal.msgSuccess('新增成功')
          open.value = false
          getList()
        })
      }
    }
  })
}

// 删除按钮操作
function handleDelete(row) {
  const Ids = row.chapeterId || ids.value

  proxy
    .$confirm('是否确认删除参数编号为"' + Ids + '"的数据项？', '警告', {
      confirmButtonText: proxy.$t('common.ok'),
      cancelButtonText: proxy.$t('common.cancel'),
      type: 'warning'
    })
    .then(function () {
      return delChapeter(Ids)
    })
    .then(() => {
      getList()
      proxy.$modal.msgSuccess('删除成功')
    })
}

// 导入数据成功处理
const handleFileSuccess = (response) => {
  const { item1, item2 } = response.data
  var error = ''
  item2.forEach((item) => {
    error += item.storageMessage + ','
  })
  proxy.$alert(item1 + '<p>' + error + '</p>', '导入结果', {
    dangerouslyUseHTMLString: true
  })
  getList()
}
onMounted(() => {
  OptionsByCourse()
})
handleQuery()
</script>
