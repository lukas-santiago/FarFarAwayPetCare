import { useContext, useEffect, useState } from 'react'
import { Button, Container } from 'react-bootstrap'
import { useNavigate } from 'react-router-dom'
import { ErrorBoundary } from '../../components/ErrorBoundary'
import { GenericTable } from '../../components/GenericTable'
import GlobalContext from '../../contexts/global'
import { getDevices } from '../../services/api'
import { Device } from '../../types/types'

const columns = ['#', 'Nome', 'ID do Dispositivo']

export function ConfiguracaoDispositivoRoute(): any {
  const ctx = useContext(GlobalContext)
  const navigate = useNavigate()
  const [deviceData, setDeviceData] = useState<Device[]>([])

  useEffect(() => {
    if (JSON.stringify(ctx.user) === JSON.stringify({})) navigate('/login')
    getDevices(ctx.user.token).then((res) => setDeviceData(res.data))
  }, [])

  return (
    <Container>
      <h1 className="mb-3">Configuração dos Dispositivos</h1>
      <hr />
      <section>
        <ErrorBoundary>
          <div className="d-flex justify-content-end">
            <Button size="sm" variant="secondary">
              Adicionar Dispositivo
            </Button>
          </div>
          <GenericTable<Device>
            columns={columns}
            data={deviceData}
            actions={undefined}
            handleRow={(event: any, iRow: number) => {
              navigate('/configuracao/dispositivos/' + deviceData[iRow].id)
            }}
          />
        </ErrorBoundary>
      </section>
    </Container>
  )
}
