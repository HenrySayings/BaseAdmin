<!--
 * @Descripttion: (选择题/多选题/Select)
 * @Author: (admin)
 * @Date: (2024-05-11)
-->
<template>
  <div>
    <el-form :model="queryParams" label-position="right" inline ref="queryRef" v-show="showSearch" @submit.prevent>
      <el-form-item>
        <el-button icon="refresh" @click="resetQuery">{{ $t('btn.reset') }}</el-button>
        <el-button type="info" plain icon="Upload" @click="handleImport" v-hasPermi="['system:user:import']">
          {{ $t('btn.import') }}
        </el-button>
      </el-form-item>
    </el-form>
    <!-- 工具区域 -->
    <el-row :gutter="15" class="mb10">
      <el-col :span="1.5">
        <el-button type="primary" v-hasPermi="['select:add']" plain icon="plus" @click="handleAdd">
          {{ $t('btn.add') }}
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
      @sort-change="sortChange">
      <el-table-column prop="question" label="题目" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('question')" />
      <el-table-column prop="answer" label="答案" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('answer')" />
      <el-table-column prop="chapter" label="章节" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('chapter')" />
      <el-table-column prop="optionA" label="选择A" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('optionA')" />
      <el-table-column prop="optionB" label="选择B" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('optionB')" />
      <el-table-column prop="optionC" label="选择C" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('optionC')" />
      <el-table-column prop="optionD" label="选择D" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('optionD')" />
      <el-table-column prop="ideas" label="Ideas" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('ideas')" />
      <el-table-column prop="CreateUser" label="创建人" align="center" :show-overflow-tooltip="true" v-if="columns.showColumn('CreateUser')" />
      <el-table-column prop="CreateTime" label="创建时间" :show-overflow-tooltip="true" v-if="columns.showColumn('CreateTime')" />
      <el-table-column label="操作" width="160">
        <template #default="scope">
          <el-button type="success" size="small" icon="edit" title="编辑" v-hasPermi="['select:edit']" @click="handleUpdate(scope.row)"></el-button>
          <el-button
            type="danger"
            size="small"
            icon="delete"
            title="删除"
            v-hasPermi="['select:delete']"
            @click="handleDelete(scope.row)"></el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination :total="total" v-model:page="queryParams.pageNum" v-model:limit="queryParams.pageSize" @pagination="getList" />
    <el-dialog :title="upload.title" v-model="upload.open" width="400px" append-to-body>
      <el-upload
        name="file"
        ref="uploadRef"
        :limit="1"
        accept=".xlsx,.xls"
        :headers="upload.headers"
        :action="upload.url + '?updateSupport=' + upload.updateSupport"
        :disabled="upload.isUploading"
        :on-progress="handleFileUploadProgress"
        :on-success="handleFileSuccess"
        :auto-upload="false"
        drag>
        <el-icon class="el-icon--upload">
          <upload-filled />
        </el-icon>
        <div class="el-upload__text">将文件拖到此处，或<em>点击上传</em></div>
        <template #tip>
          <div class="el-upload__tip text-center">
            <!-- <div class="el-upload__tip">
              <el-checkbox v-model="upload.updateSupport" /> 是否更新已经存在的用户数据
            </div> -->
            <span>仅允许导入xls、xlsx格式文件。</span>
            <el-link type="primary" :underline="false" style="font-size: 12px; vertical-align: baseline" @click="importTemplate">下载模板</el-link>
          </div>
        </template>
      </el-upload>
      <template #footer>
        <el-button @click="upload.open = false">{{ $t('btn.cancel') }}</el-button>
        <el-button type="primary" @click="submitFileForm">{{ $t('btn.submit') }}</el-button>
      </template>
    </el-dialog>
    <!-- 添加或修改选择题/多选题对话框 -->
    <el-dialog :title="title" :lock-scroll="false" v-model="open">
      <el-form ref="formRef" :model="form" :rules="rules" label-width="100px">
        <el-row :gutter="20">
          <el-col :lg="12">
            <el-form-item label="题目" prop="question">
              <el-input v-model="form.question" placeholder="请输入题目" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="答案" prop="answer">
              <el-input v-model="form.answer" placeholder="请输入答案" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="章节" prop="chapter">
              <el-select v-model="form.chapeterId" placeholder="请选择章节">
                <el-option v-for="item in optionsBy" :key="item.chapeterId" :label="item.chapterName" :value="item.chapeterId" />
              </el-select>
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="选项A" prop="optionA">
              <el-input v-model="form.optionA" placeholder="请输入选项A" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="选项B" prop="optionB">
              <el-input v-model="form.optionB" placeholder="请输入选项B" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="选项C" prop="optionC">
              <el-input v-model="form.optionC" placeholder="请输入选项C" />
            </el-form-item>
          </el-col>

          <el-col :lg="12">
            <el-form-item label="选项D" prop="optionD">
              <el-input v-model="form.optionD" placeholder="请输入选项D" />
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

<script setup name="select">
import { listSelect, addSelect, delSelect, updateSelect, getSelect,ImportSelect } from '@/api/business/select.js'
import { selectChapeter } from '@/api/business/chapeter.js'
import ImportTable from "@/views/tool/gen/importTable.vue";
import {getToken} from "@/utils/auth.js";
const { proxy } = getCurrentInstance()
const ids = ref([])
const loading = ref(false)
const showSearch = ref(true)
const queryParams = reactive({
  pageNum: 1,
  pageSize: 10,
  sort: '',
  sortType: 'asc'
})
const upload = reactive({
  // 是否显示弹出层（用户导入）
  open: false,
  // 弹出层标题（用户导入）
  title: '',
  // 是否禁用上传
  isUploading: false,
  // 是否更新已经存在的用户数据
  updateSupport: 0,
  // 设置上传的请求头部
  headers: { Authorization: 'Bearer ' + getToken() },
  // 上传的地址
  url: import.meta.env.VITE_APP_BASE_API + '/business/Select/importData'
})
function handleImport(){
  upload.title = '题库导入'
  upload.open = true
}
function importTemplate() {
  proxy.download('/business/Select/importTemplate', '题库导入模板')
}
function submitFileForm() {
  proxy.$refs['uploadRef'].submit()
}
let chapterUrl=ref();
const columns = ref([
  { visible: true, prop: 'selectId', label: 'SelectId' },
  { visible: true, prop: 'question', label: 'Question' },
  { visible: true, prop: 'answer', label: 'Answer' },
  { visible: true, prop: 'isSingle', label: 'IsSingle' },
  { visible: true, prop: 'chapter', label: 'Chapter' },
  { visible: true, prop: 'chapeterId', label: 'chapeterId' },
  { visible: true, prop: 'optionA', label: 'OptionA' },
  { visible: true, prop: 'optionB', label: 'OptionB' },
  { visible: false, prop: 'OptionC', label: 'OptionC' },
  { visible: false, prop: 'OptionD', label: 'OptionD' },
  { visible: false, prop: 'ideas', label: 'Ideas' },
  { visible: false, prop: 'CreateUser', label: 'CreateUser' },
  { visible: false, prop: 'CreateTime', label: 'CreateTime' },
  { visible: false, prop: 'updateUser', label: 'UpdateUser' },
  { visible: false, prop: 'updateTime', label: 'UpdateTime' },
  { visible: false, prop: 'isDeleted', label: 'IsDeleted' }
])
const total = ref(0)
const dataList = ref([])
const queryRef = ref()
const defaultTime = ref([new Date(2000, 1, 1, 0, 0, 0), new Date(2000, 2, 1, 23, 59, 59)])

var dictParams = []
//绑定选择框
const optionsBy = ref([])
function SelectByChapeter() {
  selectChapeter().then((res) => {
    const { code, data } = res
    if (code == 200) {
      optionsBy.value = data
      loading.value = false
    }
  })
}
function handleFileSuccess(){
  console.error(chapterUrl);
}
function beforeUpload(file) {
  const isExcel = file.name.endsWith('.xlsx') || file.name.endsWith('.xls');
  if (!isExcel) {
    ElMessage.error('只能上传 Excel 文件!');
  }
  console.error(file);
  ImportSelect(file).then(res=>{

  });
  return isExcel;
}
function handleSuccess(response, file, fileList) {
  // 处理上传成功的逻辑
  ElMessage.success('文件上传成功');
}
function getList() {
  loading.value = true
  listSelect(queryParams).then((res) => {
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

// 重置查询操作
function resetQuery() {
  proxy.resetForm('queryRef')
  handleQuery()
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
    // IsSingle 选项列表 格式 eg:{ dictLabel: '标签', dictValue: '0'}
    isSingleOptions: []
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
    selectId: null,
    question: null,
    answer: null,
    isSingle: null,
    chapter: null,
    chapterId: null,
    optionA: null,
    optionB: null,
    optionC: null,
    optionD: null,
    ideas: null,
    createUser: null,
    createTime: null,
    updateUser: null,
    updateTime: null,
    isDeleted: null
  }
  proxy.resetForm('formRef')
}

// 添加按钮操作
function handleAdd() {
  reset()
  open.value = true
  title.value = '添加选择题/多选题'
  opertype.value = 1
}
// 修改按钮操作
function handleUpdate(row) {
  reset()
  const id = row.selectId || ids.value
  getSelect(id).then((res) => {
    const { code, data } = res
    if (code == 200) {
      open.value = true
      title.value = '修改选择题/多选题'
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
      if (form.value.selectId != undefined && opertype.value === 2) {
        updateSelect(form.value).then((res) => {
          proxy.$modal.msgSuccess('修改成功')
          open.value = false
          getList()
        })
      } else {
        addSelect(form.value).then((res) => {
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
  const Ids = row.selectId || ids.value
  proxy
    .$confirm('是否确认删除参数编号为"' + Ids + '"的数据项？', '警告', {
      confirmButtonText: proxy.$t('common.ok'),
      cancelButtonText: proxy.$t('common.cancel'),
      type: 'warning'
    })
    .then(function () {
      return delSelect(Ids)
    })
    .then(() => {
      getList()
      proxy.$modal.msgSuccess('删除成功')
    })
}
onMounted(() => {
  SelectByChapeter()
})
handleQuery()
</script>
