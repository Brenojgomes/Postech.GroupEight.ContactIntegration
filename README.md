# Configuração e Implantação no Kubernetes

Este guia fornece instruções para criar os deployments dos microsserviços e configurar o monitoramento no seu ambiente Kubernetes usando Prometheus e Grafana.

## Implantação dos Microsserviços

Os arquivos necessários para criar os deployments dos microsserviços estão localizados no diretório `kubernetes` deste repositório. Para aplicar a criação de todos os deployments, execute o seguinte comando:

```bash
kubectl apply -f kubernetes/
```

Certifique-se de estar no diretório raiz do repositório ao executar este comando.

## Configuração de Monitoramento no Kubernetes

### Pré-requisitos

- Um cluster Kubernetes em execução.
- [Gerenciador de pacotes Chocolatey](https://chocolatey.org/) instalado na sua máquina local.

### Etapa 1: Instalar o Helm

Helm é um gerenciador de pacotes para Kubernetes. Para instalá-lo, execute:

```bash
choco install kubernetes-helm
```

### Etapa 2: Adicionar e Atualizar o Repositório do Prometheus

Adicione o repositório do Helm chart do Prometheus e atualize-o:

```bash
helm repo add prometheus-community https://prometheus-community.github.io/helm-charts
helm repo update
```

### Etapa 3: Instalar o Prometheus

Instale o Prometheus usando o seguinte comando:

```bash
helm install prometheus prometheus-community/prometheus \
  --namespace monitoramento \
  --create-namespace \
  --set server.service.type=LoadBalancer
```

### Etapa 4: Ajustar o Node Exporter

Execute este comando para ajustar a implantação do `prometheus-node-exporter`:

```bash
kubectl patch ds prometheus-prometheus-node-exporter -n monitoramento --type="json" -p "[{\"op\": \"remove\", \"path\": \"/spec/template/spec/containers/0/volumeMounts/2/mountPropagation\"}]"
```

### Etapa 5: Configurar o Data Source no Grafana

1. Acesse a interface web do Grafana.
2. Navegue até **Configuration > Data Sources**.
3. Adicione uma nova fonte de dados com os seguintes detalhes:

   - **Nome**: Prometheus
   - **Tipo**: Prometheus
   - **URL**: `http://prometheus-server.monitoramento.svc.cluster.local/`

4. Salve e teste a conexão com a fonte de dados.

### Notas

- Certifique-se de que o namespace `monitoramento` está acessível.
- Se encontrar problemas, verifique seu cluster Kubernetes e a instalação do Helm.